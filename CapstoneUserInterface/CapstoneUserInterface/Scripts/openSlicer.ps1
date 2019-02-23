param(
    [Parameter(Mandatory=$True)][String] $FolderPath
)

$SlicerParams = "--python-code `"import DICOMLib.DICOMUtils as utils; utils.importDicom(`"$FolderPath`")`""

If (Get-Command "Slice" -ErrorAction SilentlyContinue) {
    Slicer $SlicerParams
    "OK"
}
ElseIf (Get-Command "Slice.exe" -ErrorAction SilentlyContinue) {
    Slicer.exe $SlicerParams
    "OK"
}
Else {
    "Please add Slicer or Slicer.exe to your PATH before trying to open the folder."
}
