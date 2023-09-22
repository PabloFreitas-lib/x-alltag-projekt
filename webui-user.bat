@echo off

set PYTHON=%HOMEDRIVE%%HOMEPATH%\AppData\Local\Programs\Python\Python310\python.exe
set GIT=
set VENV_DIR=
set COMMANDLINE_ARGS=--api --xformers
cd %HOMEDRIVE%%HOMEPATH%\stable-diffusion-webui && git pull 
call webui.bat
