using NCS.DSS.CosmosDocumentClient;
using System;
using Xunit;

namespace dfc_common_pkg_netstandard_tests
{
    public class UnitTest1
    {
        
        [Fact]
        public void Test1()
        {
            // arrange
            var t = new CosmosProvider<TestClass>("Test-String");
            
            
            // act


            // assert

        }
    }

    public class TestClass
    {
        public Guid CustomerId { get; set; }
        public Guid EmploymentId { get; set; }
    }
}
