﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.Abstract;
using wshell.Objects;

namespace wshell.Core
{
    internal class ContractStore
    {
        public List<Contract> Contracts { get; } //список для хранения контрактов
        public List<RevokeTokenSource> RevokeTokens { get; } //список для хранения токенов отзыва

        public ContractStore()
        {
            Contracts = new List<Contract>();
            RevokeTokens = new List<RevokeTokenSource>();
        }

        public (Contract Contract, RevokeTokenSource RevokeTokenSource) Register(ShellBase shell, IHost host) //создает и регистрирует контракт
        {
            if (shell != null)
            {
                var revokeTokenSource = new RevokeTokenSource(shell);
                var contract = new Contract(revokeTokenSource.Token);
                Contracts.Add(contract);
                RevokeTokens.Add(revokeTokenSource);
                contract.Register(shell, host);
                return (contract, revokeTokenSource);
            }
            return (null, null);
        }
        public void Revoke(ShellBase shell) //отзывает контракт
        {
            var revokeTokenSource = RevokeTokens.FirstOrDefault(x => x.Token.ShellId == shell.ShellInfo.Id);
            if (revokeTokenSource != null)
            {
                revokeTokenSource.Revoke();
                RevokeTokens.Remove(revokeTokenSource);
                revokeTokenSource = null;
            }
            var contract = Contracts.FirstOrDefault(x => x.DestinationShellId == shell.ShellInfo.Id);
            if (contract != null)
            {
                Contracts.Remove(contract);
                contract = null;
            }
            else
            {
                throw new Exception("Contract do not exist");
            }
        }

        public void RevokeRange(IEnumerable<ShellBase> shells) //отзывает множество контрактов
        {
            foreach(var shell in shells)
            {
                Revoke(shell);
            }
        }

        public (Contract Contract, RevokeTokenSource RevokeTokenSource) Get(ShellBase shell) //получает контракт и токен отзыва
        {
            var revokeTokenSource = RevokeTokens.FirstOrDefault(x => x.Token.ShellId == shell.ShellInfo.Id);
            var contract = Contracts.FirstOrDefault(x => x.DestinationShellId == shell.ShellInfo.Id);
            return (contract, revokeTokenSource);
        }
    }
}
