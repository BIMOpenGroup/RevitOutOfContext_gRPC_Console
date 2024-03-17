# RevitOutOfContext_gRPC_Console
<h4 align="center">A minimal CLI server to run remote commands in Revit built on .NET .NET Framework</h4>

<p align="center">
  <a href="#key-features">Key Features</a> •
  <a href="#how-to-use">How To Use</a> •
  <a href="#related">Support</a> •
  <a href="#license">License</a>
</p>

## Key Features
There are three projects presented here:
* Server project ( .NET 7) 
  - [RevitOutOfContext_gRPC_Server](https://github.com/BIMOpenGroup/RevitOutOfContext_gRPC_Console/tree/master/RevitOutOfContext_gRPC_Server) 

* Revit plugin (.NET Framework 4.8) project that checks 
for the presence of an active server and requests commands from it
  - [RevitOutOfContext_gRPC_Client](https://github.com/BIMOpenGroup/RevitOutOfContext_gRPC_Console/tree/master/RevitOutOfContext_gRPC_Client)

* Data schema (google.protobuf)
  - [RevitOutOfContext_gRPC_ProtosF](RevitOutOfContext_gRPC_Protos/RevitOutOfContext_gRPC_ProtosF)

## How To Use
To clone and run this application, you'll need Git and Visual Studio installed on your computer.

* Clone this repository
$ git clone https://github.com/BIMOpenGroup/RevitOutOfContext_gRPC_Console.git

* Open project`s in VS

* Install dependencies from Nuget

* Compile all projects 

* In RevitAddinOutOfContext_gRPC_Client.addin change dll path 
 
* Copy Revit addin file RevitAddinOutOfContext_gRPC_Client.addin to your Revit plugins folder 

* Run Server from VS

* Send commands 

* Enjoy :)

## Support
Buy Me A Coffee

# License
MIT