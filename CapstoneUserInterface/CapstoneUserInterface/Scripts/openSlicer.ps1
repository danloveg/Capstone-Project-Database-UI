param(
    [Parameter(Mandatory=$True)][String] $FolderPath
)

$PythonCode = "import DICOMLib.DICOMUtils as utils; utils.importDicom('{0}')" -f $FolderPath

If (Get-Command "Slicer" -ErrorAction SilentlyContinue) {
    Slicer --python-code  $PythonCode
    "OK"
}
ElseIf (Get-Command "Slicer.exe" -ErrorAction SilentlyContinue) {
    Slicer.exe --python-code $PythonCode
    "OK"
}
Else {
    "Please add Slicer or Slicer.exe to your PATH before trying to open the folder."
}
