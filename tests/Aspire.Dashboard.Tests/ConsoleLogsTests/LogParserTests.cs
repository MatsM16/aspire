// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.ConsoleLogs;
using Xunit;

namespace Aspire.Dashboard.Tests.ConsoleLogsTests;

/// <summary>
/// This class is intended to test <see cref="LogParser"/> as a whole,
/// not the individual parsers.
/// </summary>
public class LogParserTests
{
    [Theory]
    [InlineData("No urls", "No urls")]
    [InlineData("Named https url: https://some-page.com/", "Named https url: <a target=\"_blank\" href=\"https://some-page.com/\">https://some-page.com/</a>")]
    [InlineData("Named http url: http://some-page.com/", "Named http url: <a target=\"_blank\" href=\"http://some-page.com/\">http://some-page.com/</a>")]
    [InlineData("IP https url: https://0.0.0.0/", "IP https url: <a target=\"_blank\" href=\"https://0.0.0.0/\">https://0.0.0.0/</a>")]
    [InlineData("IP https url: http://0.0.0.0/", "IP https url: <a target=\"_blank\" href=\"http://0.0.0.0/\">http://0.0.0.0/</a>")]
    [InlineData("Named https url with port: https://some-page.com:5173/", "Named https url with port: <a target=\"_blank\" href=\"https://some-page.com:5173/\">https://some-page.com:5173/</a>")]
    [InlineData("Named http url with port: http://some-page.com:5173/", "Named http url with port: <a target=\"_blank\" href=\"http://some-page.com:5173/\">http://some-page.com:5173/</a>")]
    [InlineData("IP https url with port: https://0.0.0.0:5173/", "IP https url with port: <a target=\"_blank\" href=\"https://0.0.0.0:5173/\">https://0.0.0.0:5173/</a>")]
    [InlineData("IP http url with port: http://0.0.0.0:5173/", "IP http url with port: <a target=\"_blank\" href=\"http://0.0.0.0:5173/\">http://0.0.0.0:5173/</a>")]
    [InlineData("Named https url: https://some-page.com", "Named https url: <a target=\"_blank\" href=\"https://some-page.com\">https://some-page.com</a>")]
    [InlineData("Named http url: http://some-page.com", "Named http url: <a target=\"_blank\" href=\"http://some-page.com\">http://some-page.com</a>")]
    [InlineData("IP https url: https://0.0.0.0", "IP https url: <a target=\"_blank\" href=\"https://0.0.0.0\">https://0.0.0.0</a>")]
    [InlineData("IP https url: http://0.0.0.0", "IP https url: <a target=\"_blank\" href=\"http://0.0.0.0\">http://0.0.0.0</a>")]
    [InlineData("Named https url with port: https://some-page.com:5173", "Named https url with port: <a target=\"_blank\" href=\"https://some-page.com:5173\">https://some-page.com:5173</a>")]
    [InlineData("Named http url with port: http://some-page.com:5173", "Named http url with port: <a target=\"_blank\" href=\"http://some-page.com:5173\">http://some-page.com:5173</a>")]
    [InlineData("IP https url with port: https://0.0.0.0:5173", "IP https url with port: <a target=\"_blank\" href=\"https://0.0.0.0:5173\">https://0.0.0.0:5173</a>")]
    [InlineData("IP http url with port: http://0.0.0.0:5173", "IP http url with port: <a target=\"_blank\" href=\"http://0.0.0.0:5173\">http://0.0.0.0:5173</a>")]
    public void CreateLogEntry_LinkDetectionParsesFullUrls(string input, string expectedOutput)
    {
        var logParser = new LogParser(false);
        var logEntry = logParser.CreateLogEntry(input, false);
        var output = logEntry.Content;

        Assert.Equal(expectedOutput, output);
    }
}
