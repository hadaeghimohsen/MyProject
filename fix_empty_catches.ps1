$excludeFolders = @("MessageBroadcast", "DataGuard")

$files = Get-ChildItem "D:\MyProject\System.Scsc\Ui" -Recurse -Filter "*.cs" | Where-Object {
    $exclude = $false
    foreach ($folder in $excludeFolders) {
        if ($_.FullName -match [regex]::Escape($folder)) {
            $exclude = $true
            break
        }
    }
    -not $exclude
}

$methodPattern = '^\s*(?:public|private|protected|internal)\s+(?:(?:static|virtual|override|abstract|sealed|async)\s+)*(\w+(?:\s*<[^>]+>)?)\s+(\w+)\s*\('

$totalFixes = 0
$results = @()

function Get-MethodName {
    param([string[]]$lines, [int]$catchLineIndex)
    for ($j = $catchLineIndex - 1; $j -ge 0; $j--) {
        if ($lines[$j] -match $methodPattern) {
            return $matches[2]
        }
    }
    return "Unknown"
}

function Get-Indent {
    param([string]$line)
    if ($line -match '^(\s*)') { return $matches[1] }
    return "          "
}

foreach ($file in $files) {
    $content = Get-Content -LiteralPath $file.FullName -Raw
    $lines = $content -split "`r?`n"
    $changes = 0

    $i = 0
    while ($i -lt $lines.Count) {
        $line = $lines[$i]

        # Match single-line: catch ... { }
        if ($line -match '^\s*catch\s*(\([^)]*\))?\s*\{\s*\}\s*$') {
            $methodName = Get-MethodName $lines $i
            $indent = Get-Indent $line
            $lines[$i] = "$indent" + 'catch (Exception ex) { System.Diagnostics.Debug.WriteLine("' + $methodName + ' error: " + ex.ToString()); }'
            $changes++
            $totalFixes++
            $i++
            continue
        }

        # Match multi-line: catch ... [newline] { [newline] }  (all on their own lines)
        if ($line -match '^\s*catch\s*(\([^)]*\))?\s*$') {
            $nextLine = if ($i + 1 -lt $lines.Count) { $lines[$i + 1] } else { "" }
            $nextNextLine = if ($i + 2 -lt $lines.Count) { $lines[$i + 2] } else { "" }

            if (($nextLine -match '^\s*\{\s*$') -and ($nextNextLine -match '^\s*\}\s*$')) {
                $methodName = Get-MethodName $lines $i
                $indent = Get-Indent $line
                $lines[$i] = "$indent" + 'catch (Exception ex) { System.Diagnostics.Debug.WriteLine("' + $methodName + ' error: " + ex.ToString()); }'
                $lines[$i + 1] = ""
                $lines[$i + 2] = ""
                $changes++
                $totalFixes++
                $i += 3
                continue
            }
        }

        $i++
    }

    if ($changes -gt 0) {
        # Remove blank lines that were left from multi-line removal
        $cleaned = @()
        $prevBlank = $false
        foreach ($l in $lines) {
            $isBlank = ($l -match '^\s*$')
            if ($isBlank -and $prevBlank) { } else { $cleaned += $l }
            $prevBlank = $isBlank
        }
        $lines = $cleaned

        $results += "$($file.FullName): $changes"
        Set-Content -LiteralPath $file.FullName -Value ($lines -join "`r`n") -NoNewline
    }
}

Write-Output "=== SUMMARY ==="
Write-Output "Total files modified: $($results.Count)"
Write-Output "Total catches fixed: $totalFixes"
Write-Output ""
$results | ForEach-Object { Write-Output $_ }
