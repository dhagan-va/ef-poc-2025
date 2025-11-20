@echo off
echo Starting Moto Server in the background...
start /B cmd /k "python -m moto.server -p 7808"

echo Giving the server a moment to start...
timeout /t 3 /nobreak >nul

echo Uploading EDI test data...
python s3_setup.py

echo Setup complete. Server is still running.
echo Press Ctrl+C in the separate Moto Server window to stop it later.

REM Optional: Start your application here
REM python your_app_under_test.py