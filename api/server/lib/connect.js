const mysql = require('mysql');
//Connect to mysql
module.exports = {
			Connect : (base)=>{
				var con = mysql.createConnection({
					host: 		"localhost",
					user: 		"juanbosc_admin",
					password:	"Frizzer1234",
					database:	base
				})
				con.connect(err => {
				  if (err) {console.log (err); return;};
				  console.log("Connected to "+base);
				});
				return con;
			}
}
