using System;
using System.Threading.Tasks;
using VNogin.FluentAssertions.FakeDbProvider;
using Xunit;
using Xunit.Sdk;
using FluentAssertions;

namespace Goodwin.John.Fakes.FakeDbProvider.Tests
{
    public class FakeDbConnectionAssertionsTests
    {
        readonly FakeDbConnection fakedb = new FakeDbConnection("");

        [Fact]
        public async Task HaveSingleOpenWithTransaction_Test()
        {
            //assert
            await fakedb.OpenAsync();
            var trans = await fakedb.BeginTransactionAsync();
            await trans.CommitAsync();
            await trans.DisposeAsync();
            await fakedb.CloseAsync();
            await fakedb.DisposeAsync();

            //action
            Action action = () => fakedb.Should().HaveSingleOpenWithTransaction();

            // asserts
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(false, false, false, false, false)]
        [InlineData(true, false, false, false, false)]
        [InlineData(true, true, false, false, false)]
        [InlineData(true, true, true, false, false)]
        [InlineData(true, true, true, true, false)]
        [InlineData(true, false, false, true, true)]
        public async Task MustHaveSingleOpenWithTransaction_ShouldFail(
            bool open, bool transBegin, bool commit, bool close, bool dispose)
        {
            //set up
            if (open) {
                await fakedb.OpenAsync();
                if (transBegin) {
                    var trans = await fakedb.BeginTransactionAsync();
                    if (commit)
                        await trans.CommitAsync();
                    await trans.DisposeAsync();
                }

                if (close)
                    await fakedb.CloseAsync();

                if (dispose)
                    await fakedb.DisposeAsync();
            }

            // action
            Action action = () => fakedb.Should().HaveSingleOpenWithTransaction();

            //assert
            action.Should().Throw<XunitException>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task MustHaveSingleOpenWithTransactionRollback_ShouldSuccess(bool doRollback)
        {
            //assert
            await fakedb.OpenAsync();
            var trans = await fakedb.BeginTransactionAsync();
            if (doRollback)
                await trans.RollbackAsync();
            await trans.DisposeAsync();
            await fakedb.CloseAsync();
            await fakedb.DisposeAsync();

            //action
            Action action = () => fakedb.Should().HaveSingleOpenWithTransactionRollback();

            // asserts
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(false, false, false, false, false)]
        [InlineData(true, false, false, false, false)]
        [InlineData(true, true, false, false, false)]
        [InlineData(true, true, true, false, false)]
        [InlineData(true, true, true, true, false)]
        [InlineData(true, false, false, true, true)]
        public async Task MustHaveSingleOpenWithTransactionRollback_ShouldFail(
            bool open, bool transBegin, bool commit, bool close, bool dispose)
        {
            //set up
            if (open) {
                await fakedb.OpenAsync();
                if (transBegin) {
                    var trans = await fakedb.BeginTransactionAsync();
                    if (commit)
                        await trans.CommitAsync();
                    await trans.DisposeAsync();
                }

                if (close)
                    await fakedb.CloseAsync();

                if (dispose)
                    await fakedb.DisposeAsync();
            }

            // action
            Action action = () => fakedb.Should().HaveSingleOpenWithTransactionRollback();

            //assert
            action.Should().Throw<XunitException>();
        }
    }
}
