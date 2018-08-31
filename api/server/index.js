const express				= require("express");
const bodyParser		= require("body-parser");
const con 					= require('./lib/connect.js').Connect("juanbosc_snhgames");
const Router				= express.Router();

var indexFile 		= "/api/admin.html",
		dashboardFile = "/api/dashboard.html"

Router.use(bodyParser.json())
Router.use(bodyParser.urlencoded({ extended: true}))
//Routing
Router.get("/", function(req, res){
  res.redirect(indexFile)
})

Router.post("/login", function(req, res){
	let query ="Select * from users where name=?";
	con.query(query, req.body.user,
		(err, result, fields)=>{
		if(err) {
			console.log(err);
			res.redirect(indexFile)
			return;
		}
		if(!result || !result[0]){
			console.log(result);
			res.redirect(indexFile)
			return;
		}
		if(result[0].password != req.body.password){
			console.log(result[0].password);
			console.log(req.body.password);
			res.redirect(indexFile)
			return;
		}
		res.redirect(dashboardFile)
	})
})

module.exports = Router;
