#!/bin/bash

py -m venv moto_env

source moto_env/Scripts/activate

python -m pip install moto[all] flask flask_cors