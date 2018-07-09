var	http				= require ("http"),
		express			= require ("express")

var kdr					= require ("./server/KDR"),//external data
		admin				= require ("./server")
var PORT				= port = process.env.PORT || 8100 ,
		app					= express(),
		server			= http.Server(app)

app.use("/admin",admin)
app.use("/kdr",kdr)
app.use(express.static("public"))

server.listen(PORT,()=>{
	console.log("Server is running in port:" + PORT);
})
