import React, { useContext, useState } from 'react'
import {
	Avatar,
	Box,
	Button,
	Checkbox,
	Container,
	CssBaseline,
	FormControlLabel,
	Grid,
	Link,
	TextField,
	Typography
} from '@material-ui/core'
import LockOutlinedIcon from '@material-ui/icons/LockOutlined'
import { makeStyles } from '@material-ui/core/styles'
import Alert from '@material-ui/lab/Alert'
import { Redirect } from 'react-router'
import UserContext from '../../../context'

function Copyright() {
	return (
		<Typography variant="body2" color="textSecondary" align="center">
			{'Copyright © '}
			<Link color="inherit" href="/catalog">
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
	const { user, setUser } = useContext(UserContext);

	const emailRegExp = /^\S+@\S+\.\S+$/;

	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [rememberMe, setRememberMe] = useState(false);

	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);

	const handleSubmit = () => {
		if (emailRegExp.test(email)) {
			fetch('https://localhost:5001/api/account/login', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify({
					email: email,
					password: password,
					rememberMe: rememberMe,
				})
			})
				.then(response => response.json())
				.then(answer => {
					if (answer.result) {
						setMsg(answer.message);
						setUser({ userName: answer.user.userName, isAdmin: answer.user.isAdmin });
					}
					else {
						setError(answer.error);
						setUser({ userName: null, isAdmin: false });
					}
				});
		}
		else {
			setError(["Email некорректен. Проверьте правильность и повторите попытку.",]);
		}
	};

	const handleEmailChanged = (event) => {
		setEmail(event.target.value);
	};

	const handlePasswordChanged = (event) => {
		setPassword(event.target.value);
	};

	const handleChecked = (event) => {
		setRememberMe(event.target.checked);
	}

	const logoutAction = () => {
		fetch("https://localhost:5001/api/account/logoff", {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
		});
	}

	return (
		<>
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
									: user.userName !== null ? logoutAction() : null
								: <Redirect to="/catalog" />
						}
					</Box>
					<form className={classes.form}>
						<TextField
							variant="outlined"
							value={email}
							margin="normal"
							required
							fullWidth
							onChange={handleEmailChanged}
							label="Email"
							name="email"
							autoComplete="email"
							autoFocus
						/>
						<TextField
							variant="outlined"
							value={password}
							margin="normal"
							required
							fullWidth
							onChange={handlePasswordChanged}
							name="password"
							label="Пароль"
							type="password"
							id="password"
							autoComplete="current-password"
						/>
						<FormControlLabel
							control={
								<Checkbox
									value={rememberMe}
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
		</>
	);
}