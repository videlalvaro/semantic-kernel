﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using SemanticKernel.UnitTests;
using Xunit;

namespace SemanticKernel.Plugins.UnitTests.Core;

public class MathPluginTests
{
    [Fact]
    public void ItCanBeInstantiated()
    {
        // Act - Assert no exception occurs
        var _ = new MathPlugin();
    }

    [Fact]
    public void ItCanBeImported()
    {
        // Arrange
        var kernel = Kernel.Builder.Build();

        // Act - Assert no exception occurs e.g. due to reflection
        kernel.ImportSkill(new MathPlugin(), "math");
    }

    [Theory]
    [InlineData("10", "10", "20")]
    [InlineData("0", "10", "10")]
    [InlineData("0", "-10", "-10")]
    [InlineData("10", "0", "10")]
    [InlineData("-1", "10", "9")]
    [InlineData("-10", "10", "0")]
    [InlineData("-192", "13", "-179")]
    [InlineData("-192", "-13", "-205")]
    public async Task AddWhenValidParametersShouldSucceedAsync(string initialValue, string amount, string expectedResult)
    {
        // Arrange
        var target = new MathPlugin();

        // Act
        var context = await FunctionHelpers.CallViaKernel(target, "Add", ("input", initialValue), ("amount", amount));

        // Assert
        Assert.Equal(expectedResult, context.Variables.Input);
    }

    [Theory]
    [InlineData("10", "10", "0")]
    [InlineData("0", "10", "-10")]
    [InlineData("10", "0", "10")]
    [InlineData("100", "-10", "110")]
    [InlineData("100", "102", "-2")]
    [InlineData("-1", "10", "-11")]
    [InlineData("-10", "10", "-20")]
    [InlineData("-192", "13", "-205")]
    public async Task SubtractWhenValidParametersShouldSucceedAsync(string initialValue, string amount, string expectedResult)
    {
        // Arrange
        var target = new MathPlugin();

        // Act
        var context = await FunctionHelpers.CallViaKernel(target, "Subtract", ("input", initialValue), ("amount", amount));    // Assert

        // Assert
        Assert.Equal(expectedResult, context.Variables.Input);
    }

    [Theory]
    [InlineData("$0")]
    [InlineData("one hundred")]
    [InlineData("20..,,2,1")]
    [InlineData(".2,2.1")]
    [InlineData("0.1.0")]
    [InlineData("00-099")]
    [InlineData("¹²¹")]
    [InlineData("2²")]
    [InlineData("zero")]
    [InlineData("-100 units")]
    [InlineData("1 banana")]
    public async Task AddWhenInvalidInitialValueShouldThrowAsync(string initialValue)
    {
        // Arrange
        var target = new MathPlugin();

        // Act
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FunctionHelpers.CallViaKernel(target, "Add", ("input", initialValue), ("amount", "1")));

        // Assert
        AssertExtensions.AssertIsArgumentOutOfRange(ex, "value", initialValue);
    }

    [Theory]
    [InlineData("$0")]
    [InlineData("one hundred")]
    [InlineData("20..,,2,1")]
    [InlineData(".2,2.1")]
    [InlineData("0.1.0")]
    [InlineData("00-099")]
    [InlineData("¹²¹")]
    [InlineData("2²")]
    [InlineData("zero")]
    [InlineData("-100 units")]
    [InlineData("1 banana")]
    public async Task AddWhenInvalidAmountShouldThrowAsync(string amount)
    {
        // Arrange
        var target = new MathPlugin();

        // Act
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FunctionHelpers.CallViaKernel(target, "Add", ("input", "1"), ("amount", amount)));

        // Assert
        AssertExtensions.AssertIsArgumentOutOfRange(ex, "amount", amount);
    }

    [Theory]
    [InlineData("$0")]
    [InlineData("one hundred")]
    [InlineData("20..,,2,1")]
    [InlineData(".2,2.1")]
    [InlineData("0.1.0")]
    [InlineData("00-099")]
    [InlineData("¹²¹")]
    [InlineData("2²")]
    [InlineData("zero")]
    [InlineData("-100 units")]
    [InlineData("1 banana")]
    public async Task SubtractWhenInvalidInitialValueShouldThrowAsync(string initialValue)
    {
        // Arrange
        var target = new MathPlugin();

        // Act
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FunctionHelpers.CallViaKernel(target, "Subtract", ("input", initialValue), ("amount", "1")));

        // Assert
        AssertExtensions.AssertIsArgumentOutOfRange(ex, "value", initialValue);
    }

    [Theory]
    [InlineData("$0")]
    [InlineData("one hundred")]
    [InlineData("20..,,2,1")]
    [InlineData(".2,2.1")]
    [InlineData("0.1.0")]
    [InlineData("00-099")]
    [InlineData("¹²¹")]
    [InlineData("2²")]
    [InlineData("zero")]
    [InlineData("-100 units")]
    [InlineData("1 banana")]
    public async Task SubtractAsyncWhenInvalidAmountShouldThrowAsync(string amount)
    {
        // Arrange
        var target = new MathPlugin();

        // Act
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FunctionHelpers.CallViaKernel(target, "Subtract", ("input", "1"), ("amount", amount)));

        // Assert
        AssertExtensions.AssertIsArgumentOutOfRange(ex, "amount", amount);
    }
}
