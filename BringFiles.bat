@echo off
:AppEngine dll to bin
COPY /Y "D:\Projects\MySermons\AppEngine\bin\Release\AppEngine.dll" "D:\MySermons\bin\AppEngine.dll"

:AppUI dll to dll
COPY /Y "D:\Projects\MySermons\AppUI\bin\Release\AppUI.dll" "D:\MySermons\bin\AppUI.dll"

:AppLauncher exe to exe
COPY /Y "D:\Projects\MySermons\AppLauncher\bin\Release\AppLauncher.exe" "D:\MySermons\exe\AppLauncher.exe"

:UpdateCheck exe to exe
COPY /Y "D:\Projects\MySermons\Updater\bin\Release\Updater.exe" "D:\MySermons\exe\Updater.exe"

:UpdateInstaller exe to exe
COPY /Y "D:\Projects\MySermons\UpdateInstaller\bin\Release\UpdateInstaller.exe" "D:\MySermons\exe\UpdateInstaller.exe"

:MainProgram exe to parent folder
COPY /Y "D:\Projects\MySermons\MySermons\bin\Release\MySermons.exe" "D:\MySermons\MySermons.exe"

DEL "D:\Projects\MySermons\AppEngine\bin\Release\AppLauncher.exe" /Q
DEL "D:\Projects\MySermons\AppEngine\bin\Release\AppLauncher.exe.config" /Q
DEL "D:\Projects\MySermons\AppEngine\bin\Release\AppLauncher.pdb" /Q

cd "D:\MySermons"
start MySermons.exe