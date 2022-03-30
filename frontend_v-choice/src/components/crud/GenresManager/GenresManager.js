import React from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	DialogContentText,
	List,
	ListItem,
	TextField,
	Typography
} from '@material-ui/core'
import AddIcon from '@material-ui/icons/Add'
import ClearIcon from '@material-ui/icons/Clear'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'

const useStyles = makeStyles((theme) => createStyles({
	flex: {
		display: 'flex',
		justifyContent: 'space-between'
	},
	inputValue: {
		width: theme.spacing(40)
	},
	item: {
		margin: theme.spacing(1, 0)
	},
	list: {
		height: theme.spacing(50),
		overflowY: "scroll"
	}
}));

export default function FormDialog(props) {
	const classes = useStyles();

	const [open, setOpen] = React.useState(false);
	const [managerState, setState] = React.useState(
		{
			genres: [...props.genres],
			fieldTitle: "Название нового жанра",
			currentGenreId: -1,
			value: "",
			error: null,
			msg: null,
		});

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setState({ ...managerState, error: null, msg: null });
	};

	const handleSubmit = () => {
		managerState.currentGenreId === -1 ? createGenre() : updateGenre();
	};

	const checkEmptyValue = () => {

		if (!(managerState.value && managerState.value !== "")) {
			setState({ ...managerState, error: "Введено пустое значение", msg: null });
			return true;
		}

		return false;
	}

	const createGenre = () => {
		if (checkEmptyValue()) {
			return;
		}

		let genre = { value: managerState.value };

		const postURL = "https://localhost:5001/api/genre";
		fetch(postURL, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(genre)
		})
			.then(response => response.json())
			.then(data => {
				setState({
					...managerState,
					currentGenreId: -1,
					genres: [...managerState.genres, data],
					value: "",
					fieldTitle: "Название нового жанра",
					error: null,
					msg: "Жанр успешно создан",
				});
				props.onCreate(data);
			})
			.catch(_ => setState({ ...managerState, error: "Недостаточно прав для выполнения операции", msg: null }));
	};

	const updateGenre = () => {
		if (checkEmptyValue()) {
			return;
		}

		let genre = { id: managerState.currentGenreId, value: managerState.value };
		const postURL = `https://localhost:5001/api/genre/${genre.id}`;
		fetch(postURL, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(genre)
		})
			.then(response => {
				if (response.status === 401) {
					setState({ ...managerState, error: "Недостаточно прав для выполнения операции", msg: null });
				}
				if (response.status === 204) {
					let arr = [...managerState.genres];
					let found = arr.find(g => g.id === genre.id);
					if (found) {
						found.value = genre.value;
					}
					setState({
						...managerState,
						currentGenreId: -1,
						genres: [...arr],
						value: "",
						fieldTitle: "Название нового жанра",
						error: null,
						msg: "Жанр успешно изменен",
					});
					props.onUpdate(genre);
				}
			});
	};

	const handlePrepareAdd = () => {
		setState({ ...managerState, currentGenreId: -1, value: "", fieldTitle: "Название нового жанра" })
	};

	const handlePrepareUpdate = (id, value) => {
		setState({ ...managerState, currentGenreId: id, value: value, fieldTitle: "Редактирование названия" })
	};

	const handleRemoveItem = (item) => {
		fetch(`https://localhost:5001/api/genre/${item.id}`, {
			method: 'DELETE',
		})
			.then(response => {
				if (response.status === 401) {
					setState({ ...managerState, error: "Недостаточно прав для выполнения операции", msg: null });
				}
				if (response.status === 204) {
					setState({
						...managerState,
						genres: managerState.genres.filter(g => g.id !== item.id),
						error: null,
						msg: "Жанр успешно удален",
					});
					props.onDelete(item);
				}
			});
	};

	const handleChangeValue = (event) => {
		setState({ ...managerState, value: event.target.value });
	};

	return (
		<>
			<Button variant="outlined" color="primary" onClick={handleOpenDialog}>
				Управление жанрами
			</Button>

			<Dialog open={open} aria-labelledby="genres-form-dialog-title">
				<DialogTitle id="genres-form-dialog-title" variant='h6'>Диалог управления жанрами</DialogTitle>
				<DialogContent>
					<MyAlerter msg={managerState.msg} error={managerState.error} />
					<Box className={classes.item}>
						<List className={classes.list}>
							{
								managerState.genres.length !== 0
									? managerState.genres.map(genre => {
										return (
											<ListItem key={genre.id}>
												<Typography><b>{genre.value}</b></Typography>
												<Button
													variant="primary"
													onClick={() => {
														return handlePrepareUpdate(genre.id, genre.value)
													}}
												>
													Ред
												</Button>
												<Button
													variant="primary"
													onClick={() => {
														return handleRemoveItem(genre)
													}}
												>
													<ClearIcon />
												</Button>
											</ListItem>
										)
									})
									: <Typography>Список жанров пуст</Typography>
							}
						</List>
					</Box>

					<Box className={classes.item && classes.flex}>
						<TextField
							autoFocus
							margin="dense"
							id="title"
							label={managerState.fieldTitle}
							type="input"
							className={classes.inputValue}
							onChange={handleChangeValue}
							value={managerState.value}
						/>
						<Button onClick={handleSubmit} color="primary">
							Ок
						</Button>
					</Box>
				</DialogContent>
				<DialogActions className={classes.item}>
					<Button onClick={handlePrepareAdd} color="primary">
						<AddIcon />
						Режим добавления
					</Button>
					<Button onClick={handleCloseDialog} color="primary">
						Закрыть
					</Button>
				</DialogActions>
			</Dialog>
		</>
	);
}
