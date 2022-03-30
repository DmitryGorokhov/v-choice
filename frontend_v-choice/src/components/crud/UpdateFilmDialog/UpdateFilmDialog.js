import { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogContentText,
	DialogTitle,
	TextField
} from '@material-ui/core'
import CreateIcon from '@material-ui/icons/Create'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'

const useStyles = makeStyles((theme) => createStyles({
	flex: {
		display: 'flex',
		justifyContent: 'space-between'
	},
	inputTitle: {
		width: theme.spacing(40)
	},
	inputYear: {
		width: theme.spacing(10)
	},
	item: {
		margin: theme.spacing(2, 0)
	},
}));

export default function UpdateFilmDialog(props) {
	const classes = useStyles();
	const [open, setOpen] = useState(false);

	const [state, setState] = useState({
		film: { ...props.film },
		error: null,
		msg: null,
	});

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setState({ ...state, error: null, msg: null })
	};

	const handleSubmit = () => {
		const postURL = `https://localhost:5001/api/film/${props.film.id}`;
		fetch(postURL, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(state.film)
		})
			.then(response => {
				if (response.status === 401) {
					setState({ ...state, error: "Недостаточно прав для выполнения операции", msg: null });
				}
				if (response.status === 204) {
					setState({ ...state, error: null, msg: "Фильм успешно изменён" });
					props.onUpdate(state.film);
				}
			});
	};

	const handleChangeTitle = (event) => {
		setState({ ...state, film: { ...state.film, title: event.target.value } });
	};

	const handleChangeYear = (event) => {
		setState({ ...state, film: { ...state.film, year: event.target.value } });
	};

	const handleChangeDescription = (event) => {
		setState({ ...state, film: { ...state.film, description: event.target.value } });
	};

	return (
		<>
			<Button variant="outlined" color="primary" onClick={handleOpenDialog}>
				<CreateIcon />
				Изменить
			</Button>

			<Dialog open={open} /*onClose={handleCloseDialog}*/ aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Изменить информацию о фильме</DialogTitle>
				<DialogContent>
					<MyAlerter msg={state.msg} error={state.error} />
					<DialogContentText>
						Заполните информацию о фильме
					</DialogContentText>
					<Box className={classes.item && classes.flex}>
						<TextField
							autoFocus
							margin="dense"
							id="title"
							label="Название"
							type="input"
							value={state.film.title}
							className={classes.inputTitle}
							onChange={handleChangeTitle}
						/>
						<TextField
							margin="dense"
							id="year"
							label="Год"
							type="number"
							value={state.film.year}
							className={classes.inputYear}
							onChange={handleChangeYear}
						/>
					</Box>

					<TextField
						margin="dense"
						id="description"
						label="Описание"
						type="input"
						value={state.film.description}
						className={classes.item}
						onChange={handleChangeDescription}
						fullWidth
					/>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleCloseDialog} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Применить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	);
}
