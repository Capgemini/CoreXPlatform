# .NET Core Cross Platform Reference
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A repository containing an example of a cross platform .NET Core application

Build System                   | Host    | Build Agent  | Status
-------------------------------|---------|--------------|-------------------------
AppVeyor                       | Windows |     Cloud    | [![Build status](https://ci.appveyor.com/api/projects/status/3gjdbqa93gvhvq13/branch/master?svg=true)](https://ci.appveyor.com/project/jsacapdev/corexplatform/branch/master)
Travis CI                      | Trusty  |     Cloud    | [![Build Status](https://travis-ci.org/Capgemini/CoreXPlatform.svg?branch=master)](https://travis-ci.org/Capgemini/CoreXPlatform)

### features

* Creating a .NET solution using the .NET Command Line Interface (CLI), including a solution, Web API and XUnit Web Api test project
* feature 2

### Eating Cake locally

./build.ps1 --BuildNumber="1" --DockerFile="Dockerfile.windows"
./build.sh --BuildNumber="1"

## Contributing

* Fork it!
* Clone it!
* Create your feature branch: git checkout -b my-new-feature
* Commit your changes: git commit -am 'Add some feature'
* Push to the branch: git push origin my-new-feature
* Submit a pull request :D

## Credits

[License Badges](https://gist.github.com/lukas-h/2a5d00690736b4c3a7ba)

