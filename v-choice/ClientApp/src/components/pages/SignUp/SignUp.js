import React, { useState } from 'react'
import {
	Avatar,
	Box,
	Button,
	Container,
	CssBaseline,
	Grid,
	Link,
	TextField,
	Typography
} from '@material-ui/core'
import LockOutlinedIcon from '@material-ui/icons/LockOutlined'
import { makeStyles } from '@material-ui/core/styles'
import Alert from '@material-ui/lab/Alert'
import { Redirect } from 'react-router'

import { NavMenu } from '../../atoms/NavMenu/NavMenu'

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

	const logoutAction = () => {
		fetch("api/account/logoff", {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
		});
	}

	return (
		<div>
			<NavMenu />
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
								? error !== null
									? error.map((e, index) => {
										return (
											<Alert variant="outlined" severity="warning" key={index}>
												{e}
											</Alert>);
									})
									: logoutAction()
								: <Redirect to="/" />
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