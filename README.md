
# SIPRA3DPlugin - AutoCAD 2020

## Descripción
Este proyecto genera un plugin de AutoCAD con el comando `SIPRA_ESFERA`, que permite ingresar un radio y seleccionar varios puntos para generar esferas 3D sobre la capa `APANT3D`.

## Requisitos
- AutoCAD 2020
- .NET Framework 4.7.2
- Visual Studio
- Librerías: `acdbmgd.dll`, `acmgd.dll`, `AcCoreMgd.dll` (colócalas en la carpeta `libs`)

## Cómo compilar
1. Abre este proyecto en Visual Studio
2. Crea una carpeta llamada `libs` y copia dentro las 3 DLLs de AutoCAD mencionadas
3. Compila el proyecto (Release)
4. Usa `NETLOAD` en AutoCAD y selecciona el `.dll` generado
