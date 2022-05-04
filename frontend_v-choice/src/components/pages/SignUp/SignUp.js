import React, { useContext, useState } from 'react'
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
		marginTop: theme.spacing(3),
	},
	submit: {
		margin: theme.spacing(3, 0, 2),
	},
}));

export function SignUp() {
	const classes = useStyles();
	const { _, setUser } = useContext(UserContext);

	const emailRegExp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [passwordConfirm, setPasswordConfirm] = useState("");

	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);

	const handleSubmit = () => {
		if (emailRegExp.test(email)) {
			fetch('https://localhost:5001/api/account/register', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify({
					email: email,
					password: password,
					passwordConfirm: passwordConfirm,
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

	const handlePasswordConfirmChanged = (event) => {
		setPasswordConfirm(event.target.value);
	};

	const logoutAction = () => {
		fetch("https://localhost:5001/account/logoff", {
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
								: <Redirect to="/catalog" />
						}
					</Box>
					<form className={classes.form}>
						<Grid container spacing={2}>
							<Grid item xs={12}>
								<TextField
									variant="outlined"
									required
									fullWidth
									onChange={handleEmailChanged}
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
									onChange={handlePasswordChanged}
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
									onChange={handlePasswordConfirmChanged}
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
		</>
	);
}