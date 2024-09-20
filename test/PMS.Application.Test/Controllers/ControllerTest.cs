using System;
using Xunit;
using PMS.Application.Controllers;

namespace PMS.Application.Test.Controllers;

public class ConntrollerTest
{
    // xUnit uses [Fact] for test methods
    [Fact]
    public void TestingThisShouldReturnFour(){
        var quickTest = new QuickTest();
        var result = quickTest.TestingThis();
        Assert.Equal(5, result);
    }
}