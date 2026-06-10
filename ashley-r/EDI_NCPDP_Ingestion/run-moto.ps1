# PowerShell version of run-moto.sh
# Tested under PowerShell with non-admin user
 
# Create a virtual environment in the current directory
py -m venv moto_env

# Activate the virtual environment
.\moto_env\Scripts\Activate.ps1
python -m pip install --upgrade pip

# Install moto with all extras
python -m pip install moto[all] flask flask_cors

# Set the LOG_LEVEL environment variable for debugging (optional)
$env:LOG_LEVEL = "DEBUG"

# Run moto_server on port 4566
moto_server -p 4566