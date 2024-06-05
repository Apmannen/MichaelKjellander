/root/.dotnet/dotnet publish
npx tailwindcss -i bin/Release/net8.0/publish/wwwroot/MichaelKjellander.styles.css -o bin/Release/net8.0/publish/wwwroot/components.compiled.css
cp -r /root/gits/MichaelKjellander/bin/Release/net8.0/publish/* /opt/myapp/
chown -R www-data:www-data /opt/myapp
