#!/usr/bin/fish

cd webui

npm run build

and rm -r ../wwwroot/*

and cp -rf build/* ../wwwroot/
