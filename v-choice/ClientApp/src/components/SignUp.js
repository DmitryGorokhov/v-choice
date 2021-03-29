import React, { useState } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import Grid from '@material-ui/core/Grid';
import Box from '@material-ui/core/Box';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Alert from '@material-ui/lab/Alert';

function Copyright() {
	return (
		<Typography variant="body2" color="textSecondary" align="center">
			{'Copyright © '}
			<Link color="inherit" href="/">
				Viewers Choice
      </Link>{' '}
			{new Date().getFullYear()}
			{'.'}
		</Typography>
	);
}

const useStyles = makeStyles((theme) => ({
	paper: {
		marginTop: theme.spacing(8),
		display: 'flex',
		flexDirection: 'column',
		alignItems: 'center',
	},
	avatar: {
		margin: theme.spacing(1),
		backgroundColor: theme.palette.secondary.main,
	},
	form: {
		width: '100%',
		marginTop: theme.spacing(3),
	},
	submit: {
		margin: theme.spacing(3, 0, 2),
	},
}));

export function SignUp() {
	const classes = useStyles();

	let user = {
		Email: "",
		Password: "",
		PasswordConfirm: ""
	};

	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);

	const handleSubmit = () => {
		console.log(user);
		fetch('/api/account/register', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(user)
		})
			.then(response => response.json())
			.then(answer => {
				if (typeof (answer.error) === 'undefined') {
					setMsg(answer.message);
				}
				else {
					setError(answer.error);
				}
			});
	};

	const handleChanged = (event) => {
		if (event.target.name === 'email')
			user.Email = event.target.value;
		if (event.target.name === 'password')
			user.Password = event.target.value;
		if (event.target.name === 'passwordConfirm')
			user.PasswordConfirm = event.target.value;
	};

	return (
		<div>
			<Container component="main" maxWidth="xs">
				<CssBaseline />

				<div className={classes.paper}>

					<Avatar className={classes.avatar}>
						<LockOutlinedIcon />
					</Avatar>
					<Typography component="h1" variant="h5">
						Регистрация
       				</Typography>
					<Box>
						{
							msg === null
								? error && error.map(e => {
									return (
										<Alert variant="outlined" severity="warning">
											{e}
										</Alert>
									);
								})
								: () => {
									return (
										<Alert variant="outlined" severity="success">
											{msg}<br />
											<Link href="/" variant="body2">
												Перейти на главную страницу
              								</Link>
										</Alert>
									)
								}
						}
					</Box>
					<form className={classes.form} noValidate>
						<Grid container spacing={2}>
							<Grid item xs={12}>
								<TextField
									variant="outlined"
									required
									fullWidth
									onChange={handleChanged}
									id="email"
									label="Email"
									name="email"
									autoComplete="email"
								/>
							</Grid>
							<Grid item xs={12}>
								<TextField
									variant="outlined"
									required
									fullWidth
									onChange={handleChanged}
									name="password"
									label="Пароль"
									type="password"
									id="password"
									autoComplete="current-password"
								/>
							</Grid>
							<Grid item xs={12}>
								<TextField
									variant="outlined"
									required
									fullWidth
									onChange={handleChanged}
									name="passwordConfirm"
									label="Повторите пароль"
									type="password"
									id="passwordConfirm"
									autoComplete="password-confirm"
								/>
							</Grid>
						</Grid>
						<Button
							fullWidth
							variant="contained"
							color="primary"
							onClick={handleSubmit}
							className={classes.submit}
						>
							Зарегистрироваться
          				</Button>
						<Grid container justify="flex-end">
							<Grid item>
								<Link href="/sign-in" variant="body2">
									Уже есть аккаунт? Войти
              					</Link>
							</Grid>
						</Grid>
					</form>
				</div>

				<Box mt={5}>
					<Copyright />
				</Box>

			</Container>
		</div >
	);
}