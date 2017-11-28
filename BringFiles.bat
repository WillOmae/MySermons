@echo off
:AppEngine dll to bin
COPY /Y "D:\Projects\MySermons\AppEngine\bin\Debug\AppEngine.dll" "D:\MySermons\bin\AppEngine.dll"

:AppUI dll to dll
COPY /Y "D:\Projects\MySermons\AppUI\bin\Debug\AppUI.dll" "D:\MySermons\bin\AppUI.dll"

:AppLauncher exe to exe
COPY /Y "D:\Projects\MySermons\AppLauncher\bin\Debug\AppLauncher.exe" "D:\MySermons\exe\AppLauncher.exe"

:UpdateCheck exe to exe
COPY /Y "D:\Projects\MySermons\Updater\bin\Debug\Updater.exe" "D:\MySermons\exe\Updater.exe"

:UpdateInstaller exe to exe
COPY /Y "D:\Projects\MySermons\UpdateInstaller\bin\Debug\UpdateInstaller.exe" "D:\MySermons\exe\UpdateInstaller.exe"

:MainProgram exe to parent folder
COPY /Y "D:\Projects\MySermons\MySermons\bin\Debug\MySermons.exe" "D:\MySermons\MySermons.exe"

DEL "D:\Projects\MySermons\AppEngine\bin\Debug\AppLauncher.exe" /Q
DEL "D:\Projects\MySermons\AppEngine\bin\Debug\AppLauncher.exe.config" /Q
DEL "D:\Projects\MySermons\AppEngine\bin\Debug\AppLauncher.pdb" /Q

cd "D:\MySermons"
start MySermons.exe