// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.ConsoleLogs;
using Xunit;

namespace Aspire.Dashboard.Tests.ConsoleLogsTests;

public class UrlParserTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("This is some text without any urls")]
    public void TryParse_NoUrl_ReturnsFalse(string input)
    {
        var result = UrlParser.TryParse(input, out var _);

        Assert.False(result);
    }

    [Theory]
    [InlineData("This is some text with a URL at the end: https://bing.com/", true, "This is some text with a URL at the end: <a target=\"_blank\" href=\"https://bing.com/\">https://bing.com/</a>")]
    [InlineData("https://bing.com/ This is some text with a URL at the beginning", true, "<a target=\"_blank\" href=\"https://bing.com/\">https://bing.com/</a> This is some text with a URL at the beginning")]
    [InlineData("This is some text with a https://bing.com/ in the middle", true, "This is some text with a <a target=\"_blank\" href=\"https://bing.com/\">https://bing.com/</a> in the middle")]
    public void TryParse_ReturnsCorrectResult(string input, bool expectedResult, string? expectedOutput)
    {
        var result = UrlParser.TryParse(input, out var modifiedText);

        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedOutput, modifiedText);
    }

    [Theory]
    [InlineData("http://bing.com", "<a target=\"_blank\" href=\"http://bing.com\">http://bing.com</a>")]
    [InlineData("https://bing.com", "<a target=\"_blank\" href=\"https://bing.com\">https://bing.com</a>")]
    [InlineData("http://www.bing.com", "<a target=\"_blank\" href=\"http://www.bing.com\">http://www.bing.com</a>")]
    [InlineData("http://bing.com/", "<a target=\"_blank\" href=\"http://bing.com/\">http://bing.com/</a>")]
    [InlineData("http://bing.com/dir", "<a target=\"_blank\" href=\"http://bing.com/dir\">http://bing.com/dir</a>")]
    [InlineData("http://bing.com/index.aspx", "<a target=\"_blank\" href=\"http://bing.com/index.aspx\">http://bing.com/index.aspx</a>")]
    [InlineData("http://bing", "<a target=\"_blank\" href=\"http://bing\">http://bing</a>")]
    public void TryParse_SupportedUrlFormats(string input, string? expectedOutput)
    {
        var result = UrlParser.TryParse(input, out var modifiedText);

        Assert.True(result);
        Assert.Equal(expectedOutput, modifiedText);
    }

    [Theory]
    [InlineData("file:///c:/windows/system32/calc.exe")]
    [InlineData("ftp://ftp.localhost.com/")]
    [InlineData("ftp://user:pass@ftp.localhost.com/")]
    public void TryParse_UnsupportedUrlFormats(string input)
    {
        var result = UrlParser.TryParse(input, out var _);

        Assert.False(result);
    }

    [Theory]
    [InlineData("http://<span>b</span>ing.com", "<a target=\"_blank\" href=\"http://bing.com\">http://<span>b</span>ing.com</a>")]
    [InlineData("ht<span>tp</span>://<span>b</span>ing.com", "<a target=\"_blank\" href=\"http://bing.com\">ht<span>tp</span>://<span>b</span>ing.com</a>")]
    [InlineData("http://<span class=\"url-host\">bing.com</span>:81", "<a target=\"_blank\" href=\"http://bing.com:81\">http://<span class=\"url-host\">bing.com</span>:81</a>")]

    // Known limitations that must be fixed before merge
    [InlineData("<span>http</span>://bing.com:81", "<a target=\"_blank\" href=\"http://bing.com:81\"><span>http</span>://bing.com:81</a>")]
    [InlineData("http://bing.com:<span>81</span>", "<a target=\"_blank\" href=\"http://bing.com:81\">http://bing.com:<span>81</span></a>")]
    public void TryParse_IgnoresHtmlWhenMatching(string input, string output)
    {
        var result = UrlParser.TryParse(input, out var modifiedHtml);

        Assert.True(result);
        Assert.Equal(output, modifiedHtml);
    }
}
