lec=-1

set -e

while [ $lec -lt 0 ]; do
echo "Waiting for the SQL server available"
sleep 5
/opt/mssql-tools/bin/sqlcmd -H localhost \
    -U sa \
    -P 1QAZ2wsx3EDC \
    -Q "select 1 from sys.databases" \
    1> /dev/null && lec=$? || lec=-1
done

echo "Initializing the SQL server"
/opt/mssql-tools/bin/sqlcmd -H localhost \
    -U sa \
    -P 1QAZ2wsx3EDC \
    -i /var/opt/mssql-init/init-server.sql