#!/usr/bin/fish

cd webui

npm run build

and cp -rf build/* ../wwwroot/
