Branch | Build Status  | MyGet | NuGet
------ | ------- | --- | --- |
Master | [![Build Status](https://dev.azure.com/xBIMTeam/xBIMToolkit/_apis/build/status/xBimTeam.XbimExchange?branchName=master)](https://dev.azure.com/xBIMTeam/xBIMToolkit/_build/latest?definitionId=5&branchName=master) | ![master](https://img.shields.io/myget/xbim-master/v/Xbim.Exchange.svg) | ![](https://img.shields.io/nuget/v/Xbim.Exchange.svg)
Develop | [![Build Status](https://dev.azure.com/xBIMTeam/xBIMToolkit/_apis/build/status/xBimTeam.XbimExchange?branchName=develop)](https://dev.azure.com/xBIMTeam/xBIMToolkit/_build/latest?definitionId=5&branchName=develop) | ![](https://img.shields.io/myget/xbim-develop/vpre/Xbim.Exchange.svg) | -


# XbimExchange

XbimExchange is part of the [Xbim Toolkit](https://github.com/xBimTeam/XbimEssentials).

It contains libraries and applications that you can use to build applications 
that need to translate between different BIM File formats using the core XBIM Ifc
data model.

This is the home of a number of related libraries:

* [Xbim.COBie](Xbim.COBie/Xbim.Cobie.csproj) : a library to convert IFC files to COBie XLSX file
  * And a [Windows Client](Xbim.COBie.Client/Xbim.COBie.Client.csproj)
* [Xbim.Exchanger](Xbim.Exchanger/Xbim.Exchanger.csproj) : a toolkit to map between a variety of formats include COBie, COBieLite, IFC and DPoW
  * A [Windows Client](Xbim.CobieLiteUk.Client/Xbim.CobieLiteUk.Client.csproj) 
  * A [validator for COBieLiteUK files](Xbim.CobieLiteUK.Validation/Xbim.CobieLiteUK.Validation.csproj)
* XbimXplorer Plugins:
  * For [COBie Export](XplorerPlugin.COBieExport/XplorerPlugin.COBieExport.csproj)
  * For [DPoW](XplorerPlugin.DPoW/XplorerPlugin.DPoW.csproj)
  

## Compilation

**Visual Studio 2017 is recommended.**
Prior versions of Visual Studio may work, but we'd recomments 2017 where possible.
The [free VS 2017 Community Edition](https://visualstudio.microsoft.com/downloads/) will be fine. 
All projects target .NET Framework 4.7

The XBIM toolkit uses the NuGet technology for the management of several packages.
We have custom NuGet feeds for the *master* and *develop* branches of the solution, and use
Myget for CI builds. The [nuget.config](nuget.config) file should automatically add these feeds for you.


## Acknowledgements
The XbimTeam wishes to thank [JetBrains](https://www.jetbrains.com/) for supporting the XbimToolkit project 
with free open source [Resharper](https://www.jetbrains.com/resharper/) licenses.

Thanks also to Microsoft Azure DevOps for the use of [Azure Pipelines](https://azure.microsoft.com/en-us/services/devops/pipelines/) 
to automate our builds.

## Getting Involved

If you'd like to get involved and contribute to this project, please read the [CONTRIBUTING ](https://github.com/xBimTeam/XbimEssentials/blob/master/CONTRIBUTING.md) page or contact the Project Coordinators @CBenghi and @martin1cerny.
