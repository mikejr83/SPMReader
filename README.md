# SPMReader
=========

Reads Spektrum SPM Files and turns them into pretty, pretty XML.

Semi-sample output:

```xml
<SPM>
	<Spektrum Generator="DX18" VCode="1.03" Originator="HH201JAAk4AGwAGj0ELG1szlFbVB+S" PosIndex="5" PosMaxSail="10" Type="Sail" curveIndex="7" enabXPLUS="Disabled" Name="DX18 Sail 2AL 2FL V3" />
	  <Sail Wing="Ail_2_Flap_2" Tail="Normal" Motor="None" />
	</Spektrum>
</SPM>
```

## Why?

RC radio equipment has come a long way. Most enthusiasts own radios which require complex setup. Often these radios have the ability to save and restore data to some external storage. 

Spektrum brand radios save their data to a clear text file with the extension .SPM. These files contain helpful information about the programming for a specific model. 

A user can edit the information within one of these files and then restore it back to the radio.

If an XML standard was designed around the SPM file from Spektrum (and possibly other radio manufacturers' files) it would be possible to build editors, etc.
  
## Goals
--------

Ideally this project should take in any .SPM file from any Spektrum AirWave compatible receiver and convert it to a *standardized** XML output. In the long run it would be great to see a GUI editor for the XML which could in turn use this *library* to convert the XML back to a .SPM file for use in a Spektrum radio. It could also serve as a converter from one model of Spektrum radio to another or to a completely different computer radio manufacturer.

**standardized xml* - There seems to be an ordering and a structure to SPM files already. An agreeded upon DTD or XSD should be one of the goals of the project.