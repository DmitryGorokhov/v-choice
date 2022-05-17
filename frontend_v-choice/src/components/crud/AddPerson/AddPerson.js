import React, { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	DialogContentText,
	TextField,
} from '@material-ui/core'
import AddIcon from '@material-ui/icons/Add'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'

const useStyles = makeStyles((theme) => createStyles({
	item: {
		margin: theme.spacing(2, 0)
	},
	mainButton: {
		margin: theme.spacing(1)
	},
}));

function AddPerson(props) {
	const classes = useStyles();

	const [open, setOpen] = useState(false);
	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);
	const [fullName, setFullName] = useState('');
	const [image, setImage] = useState(null);

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setError(null);
		setMsg(null);
		setImage(null);
		setFullName('');
	};

	const handleSubmit = () => {
		if (fullName) {
			const formData = new FormData();
			formData.append("fullName", fullName);
			formData.append("photo", image);

			fetch('https://localhost:5001/api/person', {
				method: 'POST',
				body: formData
			})
				.then(async (response) => {
					if (response.status === 401) {
						setError("Недостаточно прав для выполнения операции");
					}
					if (response.status === 400) {
						setError("Проверьте корректность введенных данных");
					}
					if (response.status === 201) {
						const data = await response.json();
						image === null || (data && image !== null && data.photoPath !== null)
							? setMsg("Знаменитость успешно добавлена")
							: setMsg("Знаменитость добавлена без фото. Попробуйте изменить фото позже");
						props.onCreate();
					}
				});
		}
		else {
			setError("Введите ФИО");
		}
	};

	const handleChangeFullName = (event) => {
		setFullName(event.target.value);
	};

	const handleChangeImage = (event) => {
		setImage(event.target.files[0]);
	}

	return (
		<>
			<Button variant="outlined" color="primary" onClick={handleOpenDialog} className={classes.mainButton} size='small'>
				<AddIcon />
				Добавить
			</Button>
			<Dialog open={open} onClose={handleCloseDialog} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Добавить знаменитость</DialogTitle>
				<DialogContent>
					<MyAlerter msg={msg} error={error} />
					<DialogContentText>
						Заполните информацию о знаменитости
					</DialogContentText>
					<TextField
						autoFocus
						margin="dense"
						id="fullName"
						label="ФИО"
						type="input"
						className={classes.item}
						onChange={handleChangeFullName}
						value={fullName}
						fullWidth
					/>
					<input type="file" className={classes.item} onChange={handleChangeImage} />
				</DialogContent>
				<DialogActions>
					<Button onClick={handleCloseDialog} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Добавить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	)
}

export default AddPerson