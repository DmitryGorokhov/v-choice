import React, { useState } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Link from '@material-ui/core/Link';
import Grid from '@material-ui/core/Grid';
import Box from '@material-ui/core/Box';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Alert from '@material-ui/lab/Alert';
import { Redirect } from 'react-router';
import { NavMenu } from './NavMenu';

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
		marginTop: theme.spacing(1),
	},
	submit: {
		margin: theme.spacing(3, 0, 2),
	},
	center: {
		display: 'flex',
		alignItems: 'center',
	}
}));

export function SignIn() {
	const classes = useStyles();
	let user = {
		Email: "",
		Password: "",
		RememberMe: false
	};

	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);

	const handleSubmit = () => {
		fetch('api/account/login', {
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
	};

	const handleChecked = (event) => {
		user.RememberMe = event.target.checked;
	}

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
						Вход
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
						<TextField
							variant="outlined"
							margin="normal"
							required
							fullWidth
							onChange={handleChanged}
							label="Email"
							name="email"
							autoComplete="email"
							autoFocus
						/>
						<TextField
							variant="outlined"
							margin="normal"
							required
							fullWidth
							onChange={handleChanged}
							name="password"
							label="Пароль"
							type="password"
							id="password"
							autoComplete="current-password"
						/>
						<FormControlLabel
							control={
								<Checkbox
									value="remember"
									color="primary"
									onChange={handleChecked}
								/>}
							label="Запомнить меня"
						/>
						<Button
							fullWidth
							variant="contained"
							color="primary"
							onClick={handleSubmit}
							className={classes.submit}
						>
							Войти
          			</Button>
						<Grid container className="center">
							<Grid item xs >
								<Link href="/sign-up" variant="body2">
									{"Нет учетной записи? Зарегистрируйтесь"}
								</Link>
							</Grid>
						</Grid>
					</form>
				</div>
				<Box mt={8}>
					<Copyright />
				</Box>
			</Container>
		</div>
	);
}