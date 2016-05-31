var sqlite3 = require('sqlite3');
var fs = require('fs');
var program = require('commander');

program
    .version('0.0.1')
    .option('--account <value>', 'The VSTS account to read the work items from')
    .option('--username <value>', 'Your VSTS username')
    .option('--personalAccessToken <value>', 'Your VSTS personal access token')
    .option('--sqLiteDatabase <value>', 'Full path and filename for the target SQLite database')
    .option('--jsonPath <value>', 'Full path to where json files will be referenced')
    .parse(process.argv);

var config = {};
if (fs.existsSync('./config.json')) {
    var configContent = fs.readFileSync('./config.json');
    config = JSON.parse(configContent);
}

if (program.account) {
    config['account'] = program.account;
}

if (program.username) {
    config['username'] = program.username;
}

if (program.personalAccessToken) {
    config['personalAccessToken'] = program.personalAccessToken;
}

if (program.sqLiteDatabase) {
    config['sqLiteDatabase'] = program.sqLiteDatabase;

    var dbExists = fs.existsSync(program.sqLiteDatabase);
    if (!dbExists) {
        var db = new sqlite3.Database(program.sqLiteDatabase);
        db.serialize(function () {
            var createTable = `
                CREATE TABLE workItems (
                    id INTEGER NOT NULL,
                    rev INTEGER NOT NULL,
                    watermark INTEGER NOT NULL,
                    field TEXT NOT NULL,
                    value TEXT
                )`;
                
            db.run(createTable);

            var statement = db.prepare(`
                CREATE INDEX id_rev_idx ON workItems (id, rev)
            `);
            statement.run();
            statement.finalize();
        });

        db.close();
    }
}

if (program.jsonPath) {
    config['jsonPath'] = program.jsonPath;
}

fs.writeFileSync('./config.json', JSON.stringify(config, null, 4));