# ReadingRainbow
Mobile Application for Sharing and Managing Bookshelf

Instructions for Running locally - 

For ReadingRainbowAPI and ReadingRainbowAPI.Tests
    .NetCore SDK 3.1 needs to be installed
    Install Neo4jClient : dotnet add package Neo4jClient
        To run this project you will need Neo4jClient version 4.0.0 or greater: https://www.nuget.org/packages/Neo4jClient
    Install Neo4j.Driver : dotnet add package Neo4j.Driver

For ReadingRainbowAPI.Tests
    Add extention .NET Core Test Explorer to vscode to help run the tests
    Project needs to be compiled for test explorer to find tests
