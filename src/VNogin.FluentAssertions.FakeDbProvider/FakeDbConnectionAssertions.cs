using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Goodwin.John.Fakes.FakeDbProvider;

namespace VNogin.FluentAssertions.FakeDbProvider
{
    public static class FakeDbConnectionExtensions
    {
        public static FakeDbConnectionAssertions Should(this FakeDbConnection instance)
        {
            return new FakeDbConnectionAssertions(instance);
        }
    }

    public class FakeDbConnectionAssertions : ReferenceTypeAssertions<FakeDbConnection, FakeDbConnectionAssertions>
    {
        public FakeDbConnectionAssertions(FakeDbConnection instance) 
        {
            Subject = instance;
        }

        protected override string Identifier => "FakeDbConnection";

        /// <summary>
        /// Verify DbConnection has been OpenAsync, BeginTransaction, Commit, Close and Dispose
        /// </summary>
        /// <returns></returns>
        public AndWhichConstraint<FakeDbConnectionAssertions, FakeDbTransaction> HaveSingleOpenWithTransaction()
        {
            Execute.Assertion
                .ForCondition(Subject.OpenAsyncCount == 1)
                .FailWith("Expected single OpenAsync, but found {0}", Subject.OpenAsyncCount)
                .Then
                .ForCondition(Subject.CloseCount == 1)
                .FailWith("Expected single Close, but found {0}", Subject.CloseCount)
                .Then
                .ForCondition(Subject.DisposeCount == 1)
                .FailWith("Expected single dispose, but found {0}", Subject.DisposeCount)
                .Then
                .ForCondition(Subject.DbTransactions.Count == 1)
                .FailWith("Expected single transaction, but found {0}", Subject.DbTransactions.Count)
                .Then
                .Given(() => Subject.DbTransactions.SingleOrDefault())
                .ForCondition(i => i?.CommitCount == 1)
                .FailWith("Expected single transaction with commit, but found {0}", i => i?.CommitCount);
                
            return new AndWhichConstraint<FakeDbConnectionAssertions, FakeDbTransaction>(this, Subject.DbTransactions.Single());
        }

        /// <summary>
        /// Verify DbConnection has been OpenAsync, BeginTransaction, Commit, Close and Dispose
        /// </summary>
        /// <returns></returns>
        public AndConstraint<FakeDbConnectionAssertions> HaveSingleOpen()
        {
            Execute.Assertion
                .ForCondition(Subject.OpenAsyncCount == 1)
                .FailWith("Expected single OpenAsync, but found {0}", Subject.OpenAsyncCount)
                .Then
                .ForCondition(Subject.CloseCount == 1)
                .FailWith("Expected single Close, but found {0}", Subject.CloseCount)
                .Then
                .ForCondition(Subject.DisposeCount == 1)
                .FailWith("Expected single dispose, but found {0}", Subject.DisposeCount);

            return new AndConstraint<FakeDbConnectionAssertions>(this);
        }

        /// <summary>
        /// Verify DbConnection has been OpenAsync, BeginTransaction, Commit, Close and Dispose
        /// </summary>
        /// <returns></returns>
        public AndConstraint<FakeDbConnectionAssertions> HaveSingleOpenWithTransactionRollback()
        {
            Execute.Assertion
                .ForCondition(Subject.OpenAsyncCount == 1)
                .FailWith("Expected single OpenAsync, but found {0}", Subject.OpenAsyncCount)
                .Then
                .ForCondition(Subject.CloseCount == 1)
                .FailWith("Expected single Close, but found {0}", Subject.CloseCount)
                .Then
                .ForCondition(Subject.DisposeCount == 1)
                .FailWith("Expected single dispose, but found {0}", Subject.DisposeCount)
                .Then
                .ForCondition(Subject.DbTransactions.Count == 1)
                .FailWith("Expected single transaction, but found {0}", Subject.DbTransactions.Count)
                .Then
                .Given(() => Subject.DbTransactions.SingleOrDefault())
                .ForCondition(i => (i?.RollbackCount == 1 || i?.DisposeCount == 1) && i.CommitCount == 0)
                .FailWith("Expected single transaction with rollback, but found {0}", i => i?.CommitCount);

            return new AndConstraint<FakeDbConnectionAssertions>(this);
        }
    }
}
