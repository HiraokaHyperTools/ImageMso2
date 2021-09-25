# ImageMso2

## LibSaveAsPng32

```bat
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /codebase LibSaveAsPng32.dll
```

```vba
Set Class1 = CreateObject("LibSaveAsPng32.Class1")
Set Pic = CommandBars.GetImageMso("Paste", 32, 32)
Class1.SavePictureAs Pic, "C:\A\A.png"
```
