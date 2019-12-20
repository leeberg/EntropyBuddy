# EntropyBuddy
.Net core cmdline util to get Shannon Entropy'ness of a file, and AES encrypt files!

## Usage
Application can be started with command line arguments or interactively.

### Modes of Operation

* a for Encryption
* b for EntropyCheck output JUST entropy value
* c for EntropyCheck/Encrypt/EntropyCheck
* d for EntropyCheck/Encrypt/EntropyCheck/Decrypt
* e for Cross-EntropyCheck
* f for Check/Encrypt/Check, and output Pre,Post values


## Examples

* ``EntropyBuddy.Exe b "C:\mydoc.docx"``
* ``EntropyBuddy.Exe f "C:\mydoc.docx"``


## Operations in Bulk

We can use EntropyBuddy with a powershell script to scan/encrypt/scan all files in a directory. **WARNING** this will encrypt all documents in your folder and can cause irreperable data loss!

```powershell

$Results = @()
$Extensions = @(".pdf",".rtf",".docx",".dotx",".xlsx",".xlsm",".xls",".txt",".xml",".csv","msg",".doc",".md",".ppt","pptx")
$EntropyBuddy = "C:\Users\Lee\Documents\GitHub\EntropyBuddy\bin\Debug\netcoreapp3.1\EntropyBuddy.exe"

$Files = Get-ChildItem -Path "C:\Users\Lee\Desktop\AA_SDD test data - Copy" -Filter *.* -Recurse -ErrorAction SilentlyContinue -Force 

$Files | ForEach-Object {
    if($Extensions -contains $_.Extension)
    {
        $File = $_
        $FilePath = $_.FullName
        $EntropyValues = (& $EntropyBuddy b $FilePath)

        $File = [PSCustomObject]@{
            name = $File.Name
            path = $File.FullName
            extension = $File.Extension
            size = $file.Length/1KB
            entropy = $EntropyValues
        }

        "$($File.Name) - $EntropyValues"
        $Results += $File
    }
}

$TimeStamp = (Get-Date -Format("yyyy-dd-M-HH-mm-ss")).ToString()

$Results | Export-Csv -Path ( $TimeStamp + ".csv")
```
