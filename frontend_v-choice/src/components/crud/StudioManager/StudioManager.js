import React, { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
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
	},
}));

export default function StudioManager(props) {
	const classes = useStyles();

	const [open, setOpen] = useState(false);
	const [managerState, setState] = useState(
		{
			studios: [...props.studios],
			fieldTitle: "Название новой студии",
			currentStudioId: -1,
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
		managerState.currentStudioId === -1 ? createStudio() : updateStudio();
	};

	const checkEmptyValue = () => {
		if (!(managerState.value && managerState.value !== "")) {
			setState({ ...managerState, error: "Введено пустое значение", msg: null });

			return true;
		}

		return false;
	}

	const createStudio = () => {
		if (checkEmptyValue()) {
			return;
		}

		let studio = { name: managerState.value };

		const postURL = "https://localhost:5001/api/studio";
		fetch(postURL, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(studio)
		})
			.then(response => response.json())
			.then(data => {
				setState({
					...managerState,
					currentStudioId: -1,
					studios: [...managerState.studios, data],
					value: "",
					fieldTitle: "Название новой студии",
					error: null,
					msg: "Студия успешно создан",
				});
				props.onCreate(data);
			})
			.catch(_ => setState({ ...managerState, error: "Недостаточно прав для выполнения операции", msg: null }));
	};

	const updateStudio = () => {
		if (checkEmptyValue()) {
			return;
		}

		let studio = { id: managerState.currentStudioId, name: managerState.value };
		const postURL = `https://localhost:5001/api/studio/${studio.id}`;
		fetch(postURL, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(studio)
		})
			.then(response => {
				if (response.status === 401) {
					setState({ ...managerState, error: "Недостаточно прав для выполнения операции", msg: null });
				}
				if (response.status === 204) {
					let arr = [...managerState.studios];
					let found = arr.find(s => s.id === studio.id);
					if (found) {
						found.name = studio.name;
					}
					setState({
						...managerState,
						currentStudioId: -1,
						studios: [...arr],
						value: "",
						fieldTitle: "Название новой студии",
						error: null,
						msg: "Студия успешно изменен",
					});
					props.onUpdate(studio);
				}
			});
	};

	const handlePrepareAdd = () => {
		setState({ ...managerState, currentStudioId: -1, value: "", fieldTitle: "Название новой студии" })
	};

	const handlePrepareUpdate = (id, value) => {
		setState({ ...managerState, currentStudioId: id, value: value, fieldTitle: "Редактирование названия" })
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
						studios: managerState.studios.filter(s => s.id !== item.id),
						error: null,
						msg: "Студия успешно удалена",
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
			<Button variant="outlined" color="primary" onClick={handleOpenDialog} className={props.className} size='small'>
				Управление студиями
			</Button>

			<Dialog open={open} aria-labelledby="genres-form-dialog-title">
				<DialogTitle id="genres-form-dialog-title" variant='h6'>Диалог управления студиями</DialogTitle>
				<DialogContent>
					<MyAlerter msg={managerState.msg} error={managerState.error} />
					<Box className={classes.item}>
						<List className={classes.list}>
							{
								managerState.studios.length !== 0
									? managerState.studios.map(studio => {
										return (
											<ListItem key={studio.id}>
												<Typography><b>{studio.name}</b></Typography>
												<Button
													variant="primary"
													onClick={() => {
														return handlePrepareUpdate(studio.id, studio.name)
													}}
												>
													Ред
												</Button>
												<Button
													variant="primary"
													onClick={() => {
														return handleRemoveItem(studio)
													}}
												>
													<ClearIcon />
												</Button>
											</ListItem>
										)
									})
									: <Typography>Список студий пуст</Typography>
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
