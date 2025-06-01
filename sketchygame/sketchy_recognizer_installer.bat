@echo off
setlocal

:: Przywróć dostęp do narzędzi systemowych
set "PATH=%SystemRoot%\System32;%PATH%"

:: Sprawdź, czy Python istnieje
where python >nul 2>nul
if %errorlevel% neq 0 (
    echo Python nie znaleziony. Instalacja...
    goto :InstallPython
)

:: Sprawdź wersję Pythona i zapisz ją do pliku tymczasowego
python -c "import sys; print(f'{sys.version_info[0]}.{sys.version_info[1]}')" > pyver.txt
set /p PYTHON_VERSION=<pyver.txt
del pyver.txt

echo Wersja Pythona: %PYTHON_VERSION%

:: Jeśli to nie 3.12, zainstaluj
::if not "%PYTHON_VERSION%"=="3.12" (
::    echo Znaleziono niekompatybilną wersję (%PYTHON_VERSION%). ::Instalacja 3.12...
::    goto :InstallPython
::)

goto :SetupProject

:InstallPython
echo Pobieram instalator Pythona 3.12.3...
powershell -Command "Invoke-WebRequest -Uri https://www.python.org/ftp/python/3.12.3/python-3.12.3-amd64.exe -OutFile python312.exe"
start /wait python312.exe /quiet InstallAllUsers=1 PrependPath=1 Include_test=0
del python312.exe

:: Zaktualizuj PATH ręcznie (na wypadek, gdyby nie zadziałał PrependPath)
set "PATH=%ProgramFiles%\Python312;%ProgramFiles%\Python312\Scripts;%PATH%"

:: Zweryfikuj instalację
where python >nul || (
    echo Nie udalo sie zainstalowac Pythona.
    exit /b 1
)

:SetupProject
echo === Konfiguracja projektu ===

:: Dodaj Poetry do PATH jeśli nie ma
where poetry >nul
if %errorlevel% neq 0 (
    echo Instalacja Poetry...
    curl -sSL https://install.python-poetry.org/ | python -
    set "PATH=%USERPROFILE%\AppData\Roaming\Python\Scripts;%PATH%"
)

:: Sklonuj repozytorium (jeśli nie istnieje)
if not exist sketchy_recogniser (
    git clone https://github.com/maksblink/sketchy_recogniser.git
)
cd sketchy_recogniser || exit /b 1

:: Utwórz środowisko
python -m venv .venv
call .venv\Scripts\activate.bat

:: Skonfiguruj Poetry do użycia tego środowiska
poetry env use ".venv\Scripts\python.exe"

:: Instaluj zależności
poetry install

:: Uruchom program
poetry run python main.py

endlocal
pause
