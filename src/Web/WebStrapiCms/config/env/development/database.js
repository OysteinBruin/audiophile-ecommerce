module.exports =  ({ env }) => ({
	connection: {
		client: 'postgres',
		connection: {
		host: env('DATABASE_HOST', 'localhost'),
			port: env.int('DATABASE_PORT', 5432),
			database: env('DATABASE_NAME', 'strapiDB'),
			user: 'strapi',
			password: 'strapi',
			ssl: env.bool('DATABASE_SSL', false)
		}
	}
});
