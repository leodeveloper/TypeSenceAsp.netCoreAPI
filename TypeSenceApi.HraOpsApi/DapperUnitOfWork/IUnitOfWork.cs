using System;
using System.Data;

namespace TypeSenceApi.HraOpsApi.DapperUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
    }
}