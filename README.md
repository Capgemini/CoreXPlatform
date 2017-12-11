# .NET Core Cross Platform Demonstrator
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A cross platform Web API written in .NET Core using the [.NET Core SDK](https://www.microsoft.com/net/download/). This can be used as a reference solution for an application that runs on multiple operating systems including Windows, Linux and MacOS. The solution has been created using the .NET Command Line Interface (CLI) that builds and runs in [Visual Studio Code](https://code.visualstudio.com/download). The solution contains a .NET Core Web API project and a associated a XUnit test project. Other feature include:

* Cross platform build scripts using Cake 
* Cross platform CI support using Travis CI and AppVeyor (see above)
* Docker support for Linux and Windows Images, using Multi-Stage builds
* Strongly typed Theory Data for XUnit tests

### Build Status

Build System                   | Host    | Build Agent  | Status
-------------------------------|---------|--------------|-------------------------
AppVeyor                       | Windows |     Cloud    | [![Build status](https://ci.appveyor.com/api/projects/status/3gjdbqa93gvhvq13/branch/master?svg=true)](https://ci.appveyor.com/project/jsacapdev/corexplatform/branch/master)
Travis CI                      | Trusty  |     Cloud    | [![Build Status](https://travis-ci.org/Capgemini/CoreXPlatform.svg?branch=master)](https://travis-ci.org/Capgemini/CoreXPlatform)

### Eating Cake locally

To run the build scripts (written in Cake) locally, run the following in PowerShell:

`./build.ps1 --BuildNumber="1" --DockerFile="Dockerfile.windows"`

Alternatively, run the following on the bash shell:

`./build.sh --BuildNumber="1"`

## Contributing

* Fork it!
* Clone it!
* Create your feature branch: git checkout -b my-new-feature
* Commit your changes: git commit -am 'Add some feature'
* Push to the branch: git push origin my-new-feature
* Submit a pull request :D

## Credits

* https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/
* https://andrewlock.net/running-tests-with-dotnet-xunit-using-cake/

[License Badges](https://gist.github.com/lukas-h/2a5d00690736b4c3a7ba)

