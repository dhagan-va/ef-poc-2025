import os
import sys

# Run moto s3 server
os.system("python -m moto.server -p 5000")
