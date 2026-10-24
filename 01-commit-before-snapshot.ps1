# Run from the repo root (the flordevlab folder), AFTER 00-setup-episode3.ps1 and BEFORE you copy any "after" file.
# The "before" snapshot: the Ep 2 fixed code, still on raw ADO.NET.
# labshop.db is not committed (*.db is in .gitignore).
git add .

# Safety net: never commit a database file
$staged = git diff --cached --name-only | Select-String "\.db"
if ($staged) {
    Write-Host "STOP. A .db file is staged:" -ForegroundColor Yellow
    $staged
    git reset | Out-Null
    return
}

git commit -m "Patched Ep 3: start from the Ep 2 fixed code"
git tag patched-episode3-before
git push
git push origin patched-episode3-before
