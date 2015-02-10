"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\x64\sgen.exe" "SerialisationHelper.exe" /Type:Xbim.COBieLite.FacilityType /keep /force
ren *.cs XmlSerializationCode.cs
pause
del *.cmdline
del *.err
del *.out
del *.tmp

move XmlSerializationCode.cs "..\..\..\Xbim.COBieLite\COBieLite Schema"

"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\x64\sgen.exe" "SerialisationHelper.exe" /Type:Xbim.COBieLiteUK.ProjectType /keep /force
ren *.cs XmlSerializationCodeCobieUK.cs
pause
del *.cmdline
del *.err
del *.out
del *.tmp
del SerialisationHelper.XmlSerializers.dll
move XmlSerializationCodeCobieUK.cs "..\..\..\Xbim.COBieLiteUK\Schemas"