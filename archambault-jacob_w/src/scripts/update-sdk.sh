#!/bin/sh
curl -fsSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.100 --install-dir $HOME/.dotnet
export PATH="$HOME/.dotnet:$PATH"
