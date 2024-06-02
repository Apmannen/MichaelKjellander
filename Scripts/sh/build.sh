/root/.dotnet/dotnet publish
cp -r /root/gits/MichaelKjellander/bin/Release/net8.0/publish/* /opt/myapp/
chown -R www-data:www-data /opt/myapp
