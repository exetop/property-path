using System;
using System.Linq;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        public class Source
        {
            public Level1[] L1 { get; set; }
        }

        public class Level1
        {
            public Level2[] L2 { get; set; }
        }

        public class Level2
        {
            public long Id { get; set; }
        }

        [Fact]
        public void Test1()
        {
            var index1 = 5;
            var index2 = 7;
            var path = PropertyPath<Source>.Get(o => o.L1[index1].L2[index2].Id);
            Assert.Equal("L1[5].L2[7].Id", path);
        }

        [Fact]
        public void Test2()
        {
            const int index1 = 5;
            const int index2 = 7;
            var path = PropertyPath<Source>.Get(o => o.L1[index1].L2[index2].Id);
            Assert.Equal("L1[5].L2[7].Id", path);
        }

        [Fact]
        public void Test3()
        {
            var path = PropertyPath<Source>.Get(o => o.L1[5].L2[7].Id);
            Assert.Equal("L1[5].L2[7].Id", path);
        }

        [Fact]
        public void Test4()
        {
            var path = PropertyPath<Source>.Get(o => o.L1[5].L2[7]);
            Assert.Equal("L1[5].L2[7]", path);
        }

        [Fact]
        public void Test5()
        {
            var path = PropertyPath<Source>.Get(o => o.L1[5].L2);
            Assert.Equal("L1[5].L2", path);
        }

        [Fact]
        public void Test6()
        {
            var path = PropertyPath<Source>.Get(o => o.L1);
            Assert.Equal("L1", path);
        }

        [Fact]
        public void Test7()
        {
            var path = PropertyPath<Source>.Get(o => o.L1[5]);
            Assert.Equal("L1[5]", path);
        }

        public int IndexProperty => 5;

        [Fact]
        public void Test8()
        {
            var path = PropertyPath<Source>.Get(o => o.L1[IndexProperty]);
            Assert.Equal("L1[5]", path);
        }
        
        public int IndexField = 5;

        [Fact]
        public void Test9()
        {
            var path = PropertyPath<Source>.Get(o => o.L1[IndexField]);
            Assert.Equal("L1[5]", path);
        }

        [Fact]
        public void Test10()
        {
            var path = PropertyPath<Source>.Get(o => o);
            Assert.Null(path);
        }

        [Fact]
        public void Test11()
        {
            var path = PropertyPath<Source>.Get(o => o.L1.First());
            Assert.Null(path);
        }
    }
}