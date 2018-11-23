![logo](https://github.com/smchughinfo/csharpcolorspaceconverter/blob/master/logo.png)

**CONVERSIONS**

* RGB -> HSL
* RGB -> Lab
* RGB -> XYZ
* RGB -> Hex
* RGB -> "Named Color"
* HSL -> RGB
* XYZ -> Lab

**EXAMPLE USAGE**

    using CSharpColorSpaceConverter;
    var color = ColorSpaceConverter.RGBToNamedColor(114, 193, 102);
    Console.WriteLine(color);
    > Mantis (114, 193, 102)

**SOURCE**

* http://www.easyrgb.com/en/math.php
* http://www.cambiaresearch.com/c4/24c09e15-2941-4ad2-8695-00b1b4029f4d/Convert-dotnet-Color-to-Hex-String.aspx

**NUGET**
 
`PM> Install-Package ColorsSpaceConverter -Version 1.0.1`
 

**JUST GIVE ME THE FILE PLEASE**

You got it boss: [ColorSpaceConverter.cs](https://github.com/smchughinfo/csharpcolorspaceconverter/blob/master/ColorConverter/ColorSpaceConverter.cs)

**NOTES**

* The list of named colors was scraped from Wikipedia around 2013. 
* Not all conversions may appear accurate. I'm not sure how it works. I just copied the code. Something about lighting.
* If you are looking for a color picker that gives named colors I have one [here](https://github.com/smchughinfo/color).

**ABOUT**

 Over the years I've had to keep going back to easyrgb.com to lookup these conversions so I decided to put them into a repository.